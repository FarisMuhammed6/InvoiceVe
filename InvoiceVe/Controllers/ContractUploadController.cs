using InvoiceVe.DataContext;
using InvoiceVe.Entities;
using InvoiceVe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace InvoiceVe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractUploadController : ControllerBase
    {
        private readonly OcrSrvice _ocrService;
        private readonly InvoiceDbContext _dbcontext;
        public ContractUploadController(OcrSrvice ocrService, InvoiceDbContext dbcontext)
        {
            _ocrService = ocrService;
            _dbcontext = dbcontext;
        }
        [HttpPost("contract")]
        [AllowAnonymous]
        public async Task<ActionResult> UploadContractImage(IFormFile file, string contractName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            // Define the uploads directory path
            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            // Combine the path with the filename
            var filePath = Path.Combine(uploadsFolderPath, file.FileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Extract text from the image
            var extractedText = _ocrService.ExtractTextFromImage(filePath);

            // Process the extracted text
            (DateTime startDate, DateTime endDate) = ExtractContractDates(extractedText);
            var amount = ExtractAmountFromText(extractedText);

            // Create a new contract record
            var contract = new Contract
            {
                ContractName = contractName,
                StartDate = startDate,
                EndDate = endDate,
                Amount = amount
            };

            // Save the contract to the database
            _dbcontext.contracts.Add(contract);
            await _dbcontext.SaveChangesAsync();

            return Ok(contract);
        }
        private static (DateTime startDate, DateTime endDate) ExtractContractDates(string text)
        {
            var datePattern = @"\b\d{1,2}/\d{1,2}/\d{4}\b";
            var matches = Regex.Matches(text, datePattern);

            // Check if there are at least two dates
            if (matches.Count >= 2)
            {
                // Dates are: [start date, ...other dates..., end date]
                // Assuming start date is the first occurrence and end date is the last occurrence
                if (DateTime.TryParse(matches[0].Value, out DateTime startDate) &&
                    DateTime.TryParse(matches[matches.Count - 1].Value, out DateTime endDate))
                {
                    return (startDate, endDate);
                }
            }
            throw new Exception("Start date or end date not found or invalid date format.");
        }
        private decimal ExtractAmountFromText(string text)
        {
            var amountPattern = @"(?:total amount due|amount|cost|price|fee|charge|payment).*?(\$?\d+(\.\d{1,2})?)";

            var match = Regex.Match(text, amountPattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                var amountString = match.Groups[1].Value;
                amountString = amountString.Replace("$", "");

                if (decimal.TryParse(amountString, out decimal amount))
                {
                    return amount;
                }
            }
            throw new Exception("Total amount not found or invalid amount format.");
        }
    }
}

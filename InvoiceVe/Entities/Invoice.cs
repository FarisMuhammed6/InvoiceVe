﻿namespace InvoiceVe.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? ContractName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsValid { get; set; }
    }
}

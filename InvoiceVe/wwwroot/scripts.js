// scripts.js

(function () {
    'use strict';
    const forms = document.querySelectorAll('.needs-validation');

    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }

            form.classList.add('was-validated');
        }, false);
    });
})();

document.getElementById('contractUploadForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    if (!this.checkValidity()) {
        return;
    }

    const form = event.target;
    const formData = new FormData(form);

    try {
        const response = await fetch('/api/ContractUpload/contract', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const contract = await response.json();
            document.getElementById('response').innerHTML = `
                <div class="alert alert-success">
                    <strong>Success!</strong> Contract uploaded.<br>
                    Contract Name: ${contract.contractName}<br>
                    Start Date: ${new Date(contract.startDate).toLocaleDateString()}<br>
                    End Date: ${new Date(contract.endDate).toLocaleDateString()}<br>
                    Amount: $${contract.amount.toFixed(2)}
                </div>
            `;
            var successModal = new bootstrap.Modal(document.getElementById('successModal'));
            successModal.show();
        } else {
            const error = await response.text();
            document.getElementById('response').innerHTML = `
                <div class="alert alert-danger">
                    <strong>Error!</strong> ${error}
                </div>
            `;
        }
    } catch (error) {
        document.getElementById('response').innerHTML = `
            <div class="alert alert-danger">
                <strong>Error!</strong> ${error.message}
            </div>
        `;
    }
});

document.querySelector('.custom-file-input').addEventListener('change', function (event) {
    var fileName = event.target.files[0].name;
    var nextSibling = event.target.nextElementSibling;
    nextSibling.innerText = fileName;
});

const apiUrl = "/api/Employees";
let employees = [];
let employeeModal;


// Initialize application.
document.addEventListener("DOMContentLoaded", async () => {

    employeeModal = new bootstrap.Modal(
        document.getElementById("employeeModal")
    );

    document.getElementById("employeeForm")
        .addEventListener("submit", saveEmployee);

    document.getElementById("payrollForm")
        .addEventListener("submit", computePayroll);

    await loadEmployees();
});


// GET ALL EMPLOYEES
async function loadEmployees() {

    try {
        const response = await fetch(apiUrl);
        if (!response.ok) {
            throw new Error("Failed to load employees.");
        }

        employees = await response.json();
        displayEmployees();
        populatePayrollEmployees();
        document.getElementById("totalEmployees")
            .textContent = employees.length;
    }
    catch (error) {
        alert(error.message);
    }
}


// DISPLAY EMPLOYEES
function displayEmployees() {

    const tableBody =
        document.getElementById("employeeTableBody");

    tableBody.innerHTML = "";

    if (employees.length === 0) {
        const row = tableBody.insertRow();
        const cell = row.insertCell();
        cell.colSpan = 6;
        cell.className = "text-center";
        cell.textContent = "No employees found.";

        return;
    }

    employees.forEach(employee => {
        const row = tableBody.insertRow();

        const values = [
            employee.employeeNumber,
            `${employee.lastName}, ${employee.firstName}`,
            formatDate(employee.dateOfBirth),
            formatCurrency(employee.dailyRate),
            employee.workingDays
        ];

        values.forEach(value => {
            const cell = row.insertCell();

            cell.textContent = value;
        });

        const actionsCell = row.insertCell();
        const editButton =
            document.createElement("button");
        editButton.className =
            "btn btn-sm btn-warning me-2";
        editButton.textContent = "Edit";
        editButton.addEventListener("click", () => {
            editEmployee(employee.id);
        });

        const deleteButton =
            document.createElement("button");
        deleteButton.className =
            "btn btn-sm btn-danger";
        deleteButton.textContent = "Delete";
        deleteButton.addEventListener("click", () => {
            deleteEmployee(employee.id);
        });

        actionsCell.appendChild(editButton);
        actionsCell.appendChild(deleteButton);
    });
}


// OPEN CREATE MODAL
function openCreateModal() {
    document.getElementById("employeeForm").reset();
    document.getElementById("employeeId").value = "";
    document.getElementById("employeeModalTitle")
        .textContent = "Add Employee";

    employeeModal.show();
}


// OPEN EDIT MODAL
function editEmployee(id) {
    const employee = employees.find(e => e.id === id);

    if (!employee) {
        alert("Employee not found.");
        return;
    }

    document.getElementById("employeeId")
        .value = employee.id;
    document.getElementById("lastName")
        .value = employee.lastName;
    document.getElementById("firstName")
        .value = employee.firstName;
    document.getElementById("middleName")
        .value = employee.middleName || "";
    document.getElementById("dateOfBirth")
        .value = employee.dateOfBirth.substring(0, 10);
    document.getElementById("dailyRate")
        .value = employee.dailyRate;
    document.getElementById("workingDays")
        .value = employee.workingDays;
    document.getElementById("employeeModalTitle")
        .textContent = "Edit Employee";

    employeeModal.show();
}


// CREATE OR UPDATE EMPLOYEE
async function saveEmployee(event) {

    event.preventDefault();

    const id =
        document.getElementById("employeeId").value;

    const employee = {
        lastName:
            document.getElementById("lastName")
                .value.trim(),

        firstName:
            document.getElementById("firstName")
                .value.trim(),

        middleName:
            document.getElementById("middleName")
                .value.trim(),

        dateOfBirth:
            document.getElementById("dateOfBirth")
                .value,

        dailyRate:
            Number(
                document.getElementById("dailyRate")
                    .value
            ),

        workingDays:
            document.getElementById("workingDays")
                .value
    };

    const isEdit = id !== "";

    const url = isEdit
        ? `${apiUrl}/${id}`
        : apiUrl;

    const method = isEdit
        ? "PUT"
        : "POST";

    try {

        const response = await fetch(url, {
            method: method,
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(employee)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        employeeModal.hide();
        alert(
            isEdit
                ? "Employee updated successfully!"
                : "Employee created successfully!"
        );

        await loadEmployees();

        clearPayrollResult();
    }
    catch (error) {
        alert("Error saving employee: " + error.message);
    }
}


// DELETE EMPLOYEE
async function deleteEmployee(id) {

    const confirmed = confirm(
        "Are you sure you want to delete this employee?"
    );

    if (!confirmed) {
        return;
    }

    try {

        const response = await fetch(
            `${apiUrl}/${id}`,
            {
                method: "DELETE"
            }
        );

        if (!response.ok) {
            throw new Error(
                "Failed to delete employee."
            );
        }

        alert("Employee deleted successfully!");
        await loadEmployees();

        clearPayrollResult();
    }
    catch (error) {
        alert(error.message);
    }
}


// POPULATE EMPLOYEE DROPDOWN
function populatePayrollEmployees() {

    const dropdown =
        document.getElementById("payrollEmployee");

    const selectedEmployee = dropdown.value;

    dropdown.innerHTML = "";

    const defaultOption =
        document.createElement("option");

    defaultOption.value = "";

    defaultOption.textContent = "Select Employee";

    dropdown.appendChild(defaultOption);

    employees.forEach(employee => {
        const option =
            document.createElement("option");

        option.value = employee.id;

        option.textContent =
            `${employee.employeeNumber} - ` +
            `${employee.lastName}, ${employee.firstName}`;

        dropdown.appendChild(option);
    });

    if (employees.some(
        e => String(e.id) === selectedEmployee
    )) {
        dropdown.value = selectedEmployee;
    }
}


// COMPUTE TAKE-HOME PAY
async function computePayroll(event) {

    event.preventDefault();

    clearPayrollResult();

    const employeeId =
        document.getElementById("payrollEmployee").value;

    const startDate =
        document.getElementById("startDate").value;

    const endDate =
        document.getElementById("endDate").value;

    if (!employeeId) {
        alert("Please select an employee.");
        return;
    }

    if (startDate > endDate) {
        alert(
            "Start date cannot be greater than end date."
        );
        return;
    }

    const request = {
        startDate: startDate,
        endDate: endDate
    };

    try {

        const response = await fetch(
            `${apiUrl}/${employeeId}/compute-pay`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(request)
            }
        );

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        const result = await response.json();

        document.getElementById("resultEmployee")
            .textContent = result.employeeName;

        document.getElementById("resultWorkingDays")
            .textContent = result.totalWorkingDays;

        document.getElementById("resultBasicPay")
            .textContent = formatCurrency(
                result.basicPay
            );

        document.getElementById("resultBirthdayPay")
            .textContent = formatCurrency(
                result.birthdayPay
            );

        document.getElementById("resultTakeHomePay")
            .textContent = formatCurrency(
                result.takeHomePay
            );

        document.getElementById("payrollResult")
            .classList.remove("d-none");
    }
    catch (error) {
        alert(
            "Payroll computation failed: " +
            error.message
        );
    }
}


// HIDE PREVIOUS PAYROLL RESULT
function clearPayrollResult() {

    document.getElementById("payrollResult")
        .classList.add("d-none");
}

// CLEAR PAYROLL FORM AND RESULT
function clearPayroll() {
    // Reset employee, starting date, and ending date.
    document.getElementById("payrollForm").reset();
    // Hide the previous payroll computation result.
    clearPayrollResult();
}

// FORMAT MONEY
function formatCurrency(amount) {
    return new Intl.NumberFormat(
        "en-PH",
        {
            style: "currency",
            currency: "PHP"
        }
    ).format(amount);
}


// FORMAT DATE
function formatDate(dateString) {

    if (!dateString) {
        return "";
    }
    const datePart = dateString.substring(0, 10);
    const parts = datePart.split("-");
    return `${parts[1]}/${parts[2]}/${parts[0]}`;
}
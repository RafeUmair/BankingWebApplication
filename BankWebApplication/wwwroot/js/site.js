// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    $(document).ready(function () {
        $('#getAllAccountsButton').click(function () {
            $.ajax({
                url: '/api/Account/GetAllAccounts', // Replace with the actual API endpoint
                type: 'GET',
                success: function (data) {
                    // Assuming the data is an array of account objects
                    // Display the data in a table or another format
                    var tableHtml = '<table><tr><th>Account Number</th><th>Balance</th><th>First Name</th><th>Last Name</th></tr>';
                    $.each(data, function (index, account) {
                        tableHtml += '<tr><td>' + account.AcctNo + '</td><td>' + account.Bal + '</td><td>' + account.Fname + '</td><td>' + account.Lname + '</td></tr>';
                    });
                    tableHtml += '</table>';
                    $('#accountList').html(tableHtml);
                },
                error: function (error) {
                    // Handle errors here
                    console.log('Error:', error);
                }
            });
        });
    });
</script>

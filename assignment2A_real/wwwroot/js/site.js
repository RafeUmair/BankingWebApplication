// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script>
    $(document).ready(function () {
        $('.deactivate-button').click(function () {
            var username = $(this).data('username');

            if (confirm('Are you sure you want to deactivate ' + username + '?')) {
                $.post('/Admin/DeactivateUser', { username: username }, function () {
                    // Reload the page or update the UI as needed
                    location.reload(); // Refresh the page to update the user list
                });
            }
        });
    });
</script>
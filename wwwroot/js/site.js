document.addEventListener('DOMContentLoaded', function () {
    const hamburgerButton = document.getElementById('hamburger-menu');
    const sidebar = document.getElementById('sidebar');
    const closeButton = document.getElementById('close-sidebar');

    hamburgerButton.addEventListener('click', function () {
        sidebar.classList.add('active');
    });

    closeButton.addEventListener('click', function () {
        sidebar.classList.remove('active');
    });

    // Close sidebar when clicking outside
    document.addEventListener('click', function (event) {
        if (!sidebar.contains(event.target) &&
            !hamburgerButton.contains(event.target) &&
            sidebar.classList.contains('active')) {
            sidebar.classList.remove('active');
        }
    });
});


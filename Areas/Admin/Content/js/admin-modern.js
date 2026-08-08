document.addEventListener("DOMContentLoaded", function () {
    const sidebarToggle = document.querySelector("#sidebarToggle");
    const sidebar = document.querySelector("#sidebar");
    const content = document.querySelector("#content");

    sidebarToggle.addEventListener("click", function () {
        sidebar.classList.toggle("active");
        content.classList.toggle("active");
        localStorage.setItem("sidebar-collapsed", sidebar.classList.contains("active"));
    });

    // Giữ trạng thái khi reload
    if (localStorage.getItem("sidebar-collapsed") === "true") {
        sidebar.classList.add("active");
        content.classList.add("active");
    }

    // Hiệu ứng click cho sidebar
    document.querySelectorAll(".sidebar-menu a").forEach(link => {
        link.addEventListener("click", () => {
            document.querySelectorAll(".sidebar-menu a").forEach(l => l.classList.remove("active"));
            link.classList.add("active");
        });
    });
});

document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(toggle => {
    const icon = toggle.querySelector('.rotate-icon');
    const targetSelector = toggle.getAttribute('href') || toggle.dataset.bsTarget;
    const target = document.querySelector(targetSelector);
    if (target && icon) {
        target.addEventListener('show.bs.collapse', () => {
            icon.classList.add('rotate');
        });
        target.addEventListener('hide.bs.collapse', () => {
            icon.classList.remove('rotate');
        });
    }
});
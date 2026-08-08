(function () {
    // Toggle mobile menu
    $(document).on('click', '.top-nav.modern-nav .nav-toggle', function () {
        var $nav = $(this).closest('.top-nav');
        var isOpen = $nav.hasClass('open');
        $nav.toggleClass('open', !isOpen);
        $(this).attr('aria-expanded', (!isOpen).toString());
    });

    // Toggle submenus on mobile (click)
    $(document).on('click', '.top-nav.modern-nav li.has-sub > a', function (e) {
        var $nav = $(this).closest('.top-nav');
        if (!$nav.hasClass('open')) {
            // desktop: let hover handle
            return;
        }
        e.preventDefault();
        var $li = $(this).closest('li');
        $li.toggleClass('open');
    });

    // Close menu when clicking outside (mobile)
    $(document).on('click', function (e) {
        var $nav = $('.top-nav.modern-nav');
        if (!$nav.length) return;
        if ($(e.target).closest('.top-nav.modern-nav').length) return;
        $nav.removeClass('open');
        $nav.find('li.open').removeClass('open');
        $nav.find('.nav-toggle').attr('aria-expanded', 'false');
    });
})();

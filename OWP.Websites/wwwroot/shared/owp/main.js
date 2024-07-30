'use strict';

jQuery(document).ready(function ($) {
    var resizing = false,
        searchForm = $('.cd-main-search'),        
        searchTrigger = $('.cd-search-trigger'),
        coverLayer = $('.cd-cover-layer');


    function checkResize() {
        if (!resizing) {
            resizing = true;
            !window.requestAnimationFrame ? setTimeout(moveNavigation, 300) : window.requestAnimationFrame(moveNavigation);
        }
    }

    function moveNavigation() {        
        resizing = false;
    }

    function closeSearchForm() {
        searchTrigger.removeClass('search-form-visible');
        searchForm.removeClass('is-visible');
        coverLayer.removeClass('search-form-visible');
    }

    //add the .no-pointerevents class to the <html> if browser doesn't support pointer-events property
    !Modernizr.testProp('pointerEvents') && $('html').addClass('no-pointerevents');

    //move navigation and search form elements according to window width
    moveNavigation();
    $(window).on('resize', checkResize);

    //mobile version - open/close navigation
    //navigationTrigger.on('click', function (event) {
    //    event.preventDefault();
    //    mainHeader.add(navigation).add(pageContent).toggleClass('nav-is-visible');
    //});

    searchTrigger.on('click', function (event) {
        event.preventDefault();
        if (searchTrigger.hasClass('search-form-visible')) {
            searchForm.find('form').submit();
        } else {
            searchTrigger.addClass('search-form-visible');
            coverLayer.addClass('search-form-visible');
            searchForm.addClass('is-visible').one('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                searchForm.find('input[type="search"]').focus().end().off('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend');
            });
        }
    });

    //close search form
    searchForm.on('click', '.close', function () {
        closeSearchForm();
    });

    coverLayer.on('click', function () {
        closeSearchForm();
    });

    $(document).keyup(function (event) {
        if (event.which == '27') closeSearchForm();
    });

    //upadate span.selected-value text when user selects a new option
    searchForm.on('change', 'select', function () {
        searchForm.find('.selected-value').text($(this).children('option:selected').text());
    });

    $('.js-select').not('.disabled').material_select();

    $('.modal-trigger').leanModal();

    $('.chosen-select').chosen();
    $('.chosen-select-deselect').chosen({ allow_single_deselect: true });

    $(".js-tags").select2({
        tags: true
    });
    //.on("change", function (e) {
    //    $('.select2-selection__choice').each(function () {
    //        $(this).css('background', randomColor());
    //    });
    //});

    $('#exampleBasic').wizard({
        //onFinish: function () {
        //    $('#modalAlert').openModal();
        //}
    });

    //var safeColors = ['ffa60c', '2ba85a', 'ca4fe9', '2771bb', 'ef6c00', '1b5e20', '4527a0', '4e342e', '5b6abf', 'eb3f79'];
    //var rand = function () {
    //    return Math.floor(Math.random() * 10);
    //};
    //var randomColor = function () {
    //    var r = safeColors[rand()];
    //    return "#" + r;
    //};

    //$('.select2-selection__choice').each(function () {
    //    $(this).css('background', randomColor());
    //});

    $('.tooltip-tipsy').tipsy({ html: true, gravity: 's' });

    $('.js-tagsinput').tagsInput({ width: 'auto' });

    $(".note-block").mCustomScrollbar({
        autoHideScrollbar: false,
        theme: "dark-thin"
    });

    $('.js-advanceSearchButton').on('click', function (event) {
        $('#js-advanceSearch').css('transform', 'translateY(0%)');
    });

    //------------------------------------- TABS JAVASCRIPT -------------------------------------------------//

    // Intial Border Position
    var activePos = $('.tabs-header .active').position();

    // Change Position
    function changePos() {

        // Update Position
        activePos = $('.tabs-header .active').position();
        if (activePos !== undefined && activePos != null) {
            // Change Position & Width
            $('.tabs-header .border').stop().css({
                left: activePos.left,
                width: $('.tabs-header .active').width()
            });
        }
    }

    changePos();

    // Change Tab
    function changeTab() {
        var getTabId = $('.tabs-header .active a').attr('tab-id');

        // Remove Active State
        $('.tab').stop().fadeOut(300, function () {
            // Remove Class
            $(this).removeClass('active');
        }).hide();

        $('.tab[tab-id=' + getTabId + ']').stop().fadeIn(300, function () {
            // Add Class
            $(this).addClass('active');

            // Animate Height
            animateTabHeight();
        });
    }

    // Tabs
    $('.tabs-header a').on('click', function (e) {
        e.preventDefault();

        // Tab Id
        var tabId = $(this).attr('tab-id');

        // Remove Active State
        $('.tabs-header a').stop().parent().removeClass('active');

        // Add Active State
        $(this).stop().parent().addClass('active');

        changePos();

        // Update Current Itm
        tabCurrentItem = tabItems.filter('.active');

        // Remove Active State
        $('.tab').stop().fadeOut(300, function () {
            // Remove Class
            $(this).removeClass('active');
        }).hide();

        // Add Active State
        $('.tab[tab-id="' + tabId + '"]').stop().fadeIn(300, function () {
            // Add Class
            $(this).addClass('active');

            // Animate Height
            //animateTabHeight();
        });
    });

    // Tab Items
    var tabItems = $('.tabs-header ul li');

    // Tab Current Item
    var tabCurrentItem = tabItems.filter('.active');
});

$(window).load(function () {
    $('ul.select-dropdown.multiple-select-dropdown li input').each(function () {
        $(this).attr('autocomplete', 'off');

        if (!$(this).hasClass('filled-in')) {
            $(this).addClass('filled-in');
        }
    });
});

$(document).on('mouseup', function (e) {
    var sidenav = $('.side-nav'),
        toggleMenubar = $('#js-toggleMenubar');

    if (!toggleMenubar.is(e.target) && toggleMenubar.has(e.target).length === 0) {
        // if the target of the click isn't the container nor a descendant of the container
        if (!sidenav.is(e.target) && sidenav.has(e.target).length === 0 && !$('body').hasClass('site-menubar-fold')) {
            toggleMenubar.trigger('click');
        }
    }
});


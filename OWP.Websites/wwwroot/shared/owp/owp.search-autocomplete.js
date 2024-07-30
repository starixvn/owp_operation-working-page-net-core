(function ($) {
    'use strict';

    $.fn.owpSearchAutocomplete = function (methodOrOptions) {
        if ($.fn.owpSearchAutocomplete.methods[methodOrOptions]) {
            return $.fn.owpSearchAutocomplete.methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            return $.fn.owpSearchAutocomplete.methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist');
        }
    };

    //default options
    $.fn.owpSearchAutocomplete.defaultOptions = {
        url: null,
        dataMapping: { Value: 'Id', Label: 'Name' },
        maxShow: 1,
        loadDefaultItems: 0
    };

    //methods
    $.fn.owpSearchAutocomplete.methods = {
        init: function (options) {
            var opts = $.extend({}, $.fn.owpSearchAutocomplete.defaultOptions, options);

            var escapeRegex = function (value) {
                return value.replace(/[\-\[\]{}()*+?.,\\\^$|#\s]/g, "\\$&");
            };

            var filter = function (array, term) {
                var matcher = new RegExp(escapeRegex(term), "i");
                return $.grep(array, function (value) {
                    return matcher.test(value.label || value.value || value);
                });
            };

            var template = function () {
                return '<div class="owp-autocomplete-popup" style="display: none;">'
                        + '<div class="targets-table">'
                            + '<div class="table-content">'
                                + '<table>'
                                    + '<tbody><tr class="no-data"><td colspan="2">No data.</td></tr></tbody>'
                                + '</table>'
                            + '</div>'
                        + '</div>'
                        + '<div class="search-box-wrapper">'
                            + '<input type="text" class="search-box" placeholder="Search" />'
                            + '<div class="suggestions">'
                                + '<div class="suggestions-popup" style="display: none;">'
                                    + '<table>'
                                        + '<tbody></tbody>'
                                    + '</table>'
                                + '</div>'
                            + '</div>'
                        + '</div>'
                        + '<div><input type="button" class="btn-close" value="Close" /></div>'
                    + '</div>';
            };

            var registerEventRemoveLeague = function (el) {
                var $table = el.find('.targets-table .table-content table');

                $table.find('a.remove').on('click', function () {
                    var leagueId = $(this).data('leagueid'),
                        dataSelected = el.find('#DataSelected').val().split(',');

                    var index = dataSelected.indexOf(leagueId.toString());

                    if (index >= 0) {
                        dataSelected.splice(index, 1);
                    }

                    el.find('#DataSelected').val(dataSelected.join(','));

                    $(this).closest('tr').remove();

                    if ($table.find('tbody tr').length === 0) {
                        $table.find('tbody').append('<tr class="no-data"><td colspan="2">No data.</td></tr>');
                    }
                });
            };

            var addLeague = function (el, league) {
                var $tblTargets = el.find('.targets-table .table-content table'),
                    dataSelected = el.find('#DataSelected').val().length > 0 ? el.find('#DataSelected').val().split(',') : [],
                    tr = '<tr>'
                            + '<td class="col-name">' + league.Name + '</td>'
                            + '<td class="col-action">'
                                + '<a class="remove"'
                                    + ' data-leagueid="' + league.LeagueId
                                    + '">Remove</a>'
                            + '</td>'
                        + '</tr>';

                if ($tblTargets.find('tbody tr.no-data').length > 0) {
                    $tblTargets.find('tbody').html(tr);
                } else {
                    $tblTargets.find('tbody').append(tr);
                }

                dataSelected.push(league.LeagueId);
                el.find('#DataSelected').val(dataSelected.join(','));

                registerEventRemoveLeague(el);
            };

            var registerEventAddLeague = function (el) {
                var $table = el.find('.suggestions .suggestions-popup table');

                $table.find('a.add').on('click', function () {
                    var league = {
                        LeagueId: parseInt($(this).data('value')),
                        Name: $(this).closest('tr').find('.col-name').text()
                    };

                    addLeague(el, league);

                    $(this).closest('td').html('Added');
                });
            };

            var renderItemSuggestions = function (el, item) {
                var dataSelected = el.find('#DataSelected').val().length > 0 ? el.find('#DataSelected').val().split(',') : [];
                var index = dataSelected.indexOf(item[opts.dataMapping.Value].toString()),
                    colAction = index < 0
                        ? ('<a class="add" data-value="' + item[opts.dataMapping.Value] + '" href="javascript:;">Add</a>') : 'Added';

                return '<tr>'
                        + '<td class="col-name">' + item[opts.dataMapping.Label] + '</td>'
                        + '<td class="col-action">' + colAction + '</td>'
                    + '</tr>';
            };

            var renderDataSuggestions = function (el, items) {
                var table = '',
                    tbody = '';

                $.each(items, function (index, item) {
                    tbody += renderItemSuggestions(el, item);
                });

                if (tbody.length > 0) {
                    table = '<table><tbody>' + tbody + '</tbody></table>';
                } else {
                    table = '<table><tbody><tr><td colspan="2">' + 'No data.' + '</td></tr></tbody></table>'
                }

                return table;
            };

            var closePopup = function (el) {
                var $presentation = el.find('.owp-autocomplete-presentation'),
                    $tblTargets = el.find('.targets-table .table-content table');
                var leagueNames = [];

                el.find('.owp-autocomplete-popup').hide();
                el.find('.suggestions .suggestions-popup').empty();
                el.find('.suggestions .suggestions-popup').hide();
                el.find('.search-box-wrapper .search-box').val('');

                if ($tblTargets.find('tbody tr.no-data').length > 0) {
                    $presentation.find('.label-display').html('&emsp;');
                } else {
                    $tblTargets.find('tbody tr').each(function () {
                        leagueNames.push($(this).find('td.col-name').text());
                    });

                    if (leagueNames.length > opts.maxShow) {
                        $presentation.find('.label-display').text(leagueNames.length + ' items selected');
                    } else {
                        $presentation.find('.label-display').text(leagueNames.join(', '));
                    }
                }
            };

            return this.each(function () {
                var container = $(this);
                container.removeClass('owp-autocomplete').addClass('owp-autocomplete');

                container.append('<div class="owp-autocomplete-presentation">'
                        + '<span class="label-display">&emsp;</span>'
                        + '<input id="DataSelected" value="" style="display: none;" />'
                    + '</div>');

                container.append(template());

                container.find('.owp-autocomplete-presentation, .owp-autocomplete-presentation .label-display').on('click', function () {
                    container.find('.owp-autocomplete-popup').show();
                    container.find('.suggestions .suggestions-popup').empty();
                });

                container.find('.search-box-wrapper .search-box').on('input paste', function () {
                    var $popup = container.find('.suggestions .suggestions-popup'),
                        text = $(this).val(),
                        dataSelected = container.find('#DataSelected').val().length > 0 ? container.find('#DataSelected').val().split(',') : [];

                    if ($.trim(text).length > 1) {
                        $.ajax({
                            url: opts.url + '&keywords=' + $.trim(text),
                            dataType: 'json',
                            success: function (response) {
                                if (response && response.length > 0) {
                                    $popup.show();

                                    $popup.html(renderDataSuggestions(container, response));

                                    registerEventAddLeague(container);
                                }
                            },
                            error: common.HandleAjaxError
                        });
                    } else {
                        $popup.empty();
                        $popup.hide();
                    }
                });

                container.find('input.btn-close').on('click', function () {
                    closePopup(container);
                });

                $(document).on('mouseup', function (e) {
                    var $popup = container.find('.owp-autocomplete-popup'),
                        $suggestions = container.find('.suggestions .suggestions-popup');

                    if (($popup.is(e.target) || $popup.has(e.target).length > 0)
                            && (!$suggestions.is(e.target) && $suggestions.has(e.target).length === 0 && $suggestions.is(':visible'))) {
                        $suggestions.empty();
                        $suggestions.hide();
                        container.find('.search-box-wrapper .search-box').val('');
                    }

                    // if the target of the click isn't the container nor a descendant of the container
                    if (!$popup.is(e.target) && $popup.has(e.target).length === 0 && $popup.is(':visible')) {
                        closePopup(container);
                    }
                });


                if (opts.loadDefaultItems === 1) {
                    var items = container.find('.js-defaultItems').children();

                    items.each(function (id, element) {
                        var league = {
                            LeagueId: $(element).data('id'),
                            Name: $(element).data('value')
                        };

                        addLeague(container, league);
                    });

                    closePopup(container);
                }
            });
        },
        getDataSelected: function () {
            return $(this).find('#DataSelected').val();
        }
    };
})(jQuery);
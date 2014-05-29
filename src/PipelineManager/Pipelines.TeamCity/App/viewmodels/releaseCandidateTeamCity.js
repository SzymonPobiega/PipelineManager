define(['knockout', 'jquery', 'plugins/http'], function (ko, $, http) {

    var statusHtml = ko.observable();

    var refresh = function(id, tab) {
        http.get('teamcitybuilddetails/' + id).then(function (data) {
            statusHtml(data.statusHtml);
            tab.register(true, 'TeamCity');
        });
    }

    return {
        statusHtml: statusHtml,

        activate: function (tab) {
            if (!tab) {
                return;
            }
            if (tab.buildId() !== null) {
                refresh(tab.buildId(), tab);
            } else {
                tab.buildId.subscribe(function(id) {
                    refresh(id, tab);
                });
            }
        }
    };
});
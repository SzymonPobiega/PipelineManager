define(['knockout', 'jquery','plugins/http'], function (ko, $, http) {

    var historyItem = function(historyItemDto) {
        
        var style;
        if (historyItemDto.type == 'Success') {
            style = 'list-group-item-success';
        } else if (historyItemDto.type == 'Warning') {
            style = 'list-group-item-warning';
        } else {
            style = 'list-group-item-danger';
        }

        this.style = ko.observable(style);
        this.message = ko.observable(historyItemDto.message);
        this.date = ko.observable(historyItemDto.date);
    }

    return {
        items: ko.observableArray(),

        activate: function (link) {
            if (!link) {
                return;
            }
            var self = this;
            http.get(link.url).then(function (data) {
                self.items.removeAll();
                for (var i = 0; i < data.items.length; i++) {
                    self.items.push(new historyItem(data.items[i]));
                }
            });
        }
    };
});
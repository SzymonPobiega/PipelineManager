define(['knockout', 'jquery', 'plugins/http', 'json!api/extensions'], function (ko, $, http, extensions) {


    var projectName = ko.observable();
    var version = ko.observable();

    var displayName = ko.computed(function() {
        return projectName() + " " + version();
    });

    var id = ko.observable();

    var tabController = function (extensionName, buildIdObservable) {
        this.buildId = buildIdObservable;
        this.tabId = ko.observable(extensionName);
        this.title = ko.observable();
        this.visible = ko.observable(true);
        this.viewModel = ko.observable('viewmodels/' + extensionName);

        var self = this;

        this.register = function(visible, title) {
            self.visible(visible);
            self.title(title);
        }
    };

    var tabs = ko.observableArray();
    for (var i = 0; i < extensions.releaseCandidateExtensions.length; i++) {
        tabs.push(new tabController(extensions.releaseCandidateExtensions[i], id));
    }

    return {
        id : id,
        projectName: projectName,
        version : version,
        deployedTo : ko.observableArray(),
        displayName: displayName,

        testsLink: ko.observable(),
        historyLink: ko.observable(),
        deploymentsLink: ko.observable(),

        tabs: tabs,

        activate: function (locator) {
            var self = this;
            http.get(locator).then(function (data) {

                self.id(data.id);
                self.version(data.version);
                self.projectName(data.projectName);
                self.deployedTo(data.deployedTo);
                self.testsLink(data.testResults);
                self.historyLink(data.history);
                self.deploymentsLink(data.deployments);
            });
        },

        attached: function (view) {
            var $view = $(view);
            $view.find('#tabs a').click(function(e) {
                e.preventDefault();
                $(this).tab('show');
            });
        }
    };
});
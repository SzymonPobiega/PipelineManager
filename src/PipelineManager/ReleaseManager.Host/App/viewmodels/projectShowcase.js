define(['clientmodels/projectState', 'viewmodels/project', 'plugins/http'], function (projectState, project, http) {

    var projects = ko.observableArray();
    var currentIndex = ko.observable(0);

    var visibleProject = ko.computed(function () {
        if (projects().length === 0) {
            return null;
        }
        return projects()[currentIndex()].pipelineLink;
    });

    var loadProjects = function () {
        http.get('projects').then(function (model) {
            projects.removeAll();
            for (var i = 0; i < model.projects.length; i++) {
                projects.push(new projectState(model.projects[i]));
            }
        });
    }

    var rotate = function() {
        if (projects().length === 0) {
            return;
        }
        currentIndex((currentIndex() + 1) % projects().length);
    }

    var interval;

    return {
        projects: projects,
        visibleProject: visibleProject,
        currentIndex: currentIndex,

        activate: function () {
            loadProjects();            
        },

        attached: function () {
            interval = setInterval(rotate, 3000);
        },

        detached: function() {
            clearInterval(interval);
        }
    };
});
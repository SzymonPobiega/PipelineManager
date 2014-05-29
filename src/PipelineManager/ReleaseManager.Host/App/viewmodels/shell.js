define(['plugins/router', 'durandal/app'], function (router, app) {
    return {
        router: router,
        activate: function () {
            router.map([
                { route: '', title: 'Projects', moduleId: 'viewmodels/projects', nav: true },
                { route: 'showcase', title: 'Project Showcase', moduleId: 'viewmodels/projectShowcase', nav: true },
                { route: 'project/:locator', title: 'Project', moduleId: 'viewmodels/project' },
                { route: 'releasecandidate/:locator', title: 'Release Candidate', moduleId: 'viewmodels/releaseCandidate' }
            ]).buildNavigationModel();
            
            return router.activate();
        }
    };
});
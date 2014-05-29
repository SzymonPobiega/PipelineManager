define(['plugins/router'], function (router) {

    var contentTypeMap = {
        'application/vnd+releasemanager.projects+json': '',
        'application/vnd+releasemanager.pipeline+json': '#project',
        'application/vnd+releasemanager.releasecandidate+json': '#releasecandidate'
    };

    return {
        open: function (link) {
            var hashPrefix = contentTypeMap[link.type];
            var param = encodeURIComponent(link.url);
            var fragment = hashPrefix + '/' + param;
            router.navigate(fragment);
        }
    };
});
angular.module('virtoCommerce.assetVersions')
    .factory('virtoCommerce.assetVersions.resources', ['$resource', function ($resource) {
        return $resource(null, null, {
            blobVersions: { url: 'api/assetversion/blobversions', method: 'GET', isArray: true },
            downloadBlob: { url: 'api/assetversion/downloadblob', method: 'GET'},
            makeCurrentVersion: { url: 'api/assetversion/makecurrentversion', method: 'POST', params: { containerName: '@containerName', blobName: '@blobName', versionId: '@versionId' }, isArray: false }
        });
    }]);

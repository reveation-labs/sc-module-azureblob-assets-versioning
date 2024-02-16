angular.module('virtoCommerce.assetVersions')
    .controller('virtoCommerce.assetVersionsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.assetVersions.resources', 'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper',
        function ($scope, bladeNavigationService, assetApi, bladeUtils, uiGridHelper) {
        var blade = $scope.blade;
            
            blade.headIcon = 'fas fa-history';        

        blade.refresh = function () {
            blade.isLoading = true;            
            assetApi.blobVersions({ containerName: blade.conatinerName, blobName: blade.blobName },
                function (response) {                    
                    blade.isLoading = false;
                    $scope.blobVersions = response;                    
                },
                function (error) {
                    bladeNavigationService.setError('Error ' + error.status, blade);
                });
        };               

            blade.makeBlobCurrentVersion = function (blobData) {                
                blade.isLoading = true;
                assetApi.makeCurrentVersion({ containerName: blade.conatinerName, blobName: blobData.blobName, versionId: blobData.versionId },
                    function (response) {
                        blade.isLoading = false;
                    },
                    function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
            }

            blade.downloadBlob = function (blobData) {
                blade.isLoading = true;
                assetApi.downloadBlob({ containerName: blade.conatinerName, blobName: blobData.blobName, versionId: blobData.versionId },
                    function (response) {
                        blade.isLoading = false;                        
                    },
                    function (error) {
                        bladeNavigationService.setError('Error ' + error.status, blade);
                    });
            }


        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: blade.refresh,
                canExecuteMethod: function () {
                    return true;
                }
            }
        ];

        blade.title = 'assets.blades.asset-versions.title';
        blade.subtitle = 'assets.blades.asset-versions.subtitle';

        blade.refresh();        
        
        // ui-grid
        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions,
                function (gridApi) {
                    $scope.$watch('pageSettings.currentPage', gridApi.pagination.seek);
                });
        };
        bladeUtils.initializePagination($scope, true);
        
    }]);

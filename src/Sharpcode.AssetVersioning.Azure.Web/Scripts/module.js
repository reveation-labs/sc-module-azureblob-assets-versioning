// Call this to register your module to main application
var moduleName = 'virtoCommerce.assetVersions';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(
        ['platformWebApp.toolbarService', 'platformWebApp.bladeNavigationService',
            function (toolbarService, bladeNavigationService) {                
                var assetVersionCommand = {
                    name: 'assets.main-menu-title',
                    icon: 'fas fa-history',
                    index: 4,
                    executeMethod: function (blade) {
                        var selectedRow = blade.$scope.gridApi.selection.getSelectedRows()[0];                        
                        var newBlade = {
                            id: 'assetVersions',
                            conatinerName: selectedRow.relativeUrl.split("/")[1],
                            blobName: selectedRow.name,
                            title: 'assets.blades.asset-versions.title',
                            subtitle: 'assets.blades.asset-versions.subtitle',
                            controller: 'virtoCommerce.assetVersionsListController',
                            template: 'Modules/$(VirtoCommerce.AssetVersions)/Scripts/assets/blades/assetversion-list.tpl.html'
                        };

                        bladeNavigationService.showBlade(newBlade, blade);
                    },
                    canExecuteMethod: function (blade) {                        
                        return blade.$scope.gridApi && _.any(blade.$scope.gridApi.selection.getSelectedRows()) && blade.$scope.gridApi.grid.selection.lastSelectedRow.entity.type !== 'folder';
                    }
                };
                toolbarService.register(assetVersionCommand, 'virtoCommerce.assetsModule.assetListController');
            }]);

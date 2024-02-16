# Azure Blob Assets Versioing in Virto Commerce

## Overview

Virto Commerce Asset module has been improvised and sharpcode team has added a new feature for maintaining versions of assets. Versioning is pivotal feature within the Microsoft Azure platform, it provides ability to maintain and control various versions of an asset. User can restore previous version as a current version without loosing integrity of a data. Grab this new feature to have better support of user assets.

## Table of Contents
1. [Setup](#setup)
2. [User Guide](#user-guide)
3. [License](#license)

## Setup
For leveraging this feature, it require Microsoft Azure account with one time Azure blob storage setup.
Create a Azure blob storage and setup a container inside it where all the assets will be stored with versions.

### Update Virto appsettings
Update Virto appsettings.json file with detials of Azure storage account. Change provider to 'AzureBlobStorage' and provide details in AzureBlobStorage section as shown below.

![appsettings1](https://github.com/reveation-labs/sc-module-azureblob-assets-versioning/assets/115815461/e8701b37-a1f7-4f5e-9865-d0a5944545ad)

## User Guide
In Virto Commerce navigate to Assets.

![image](https://github.com/reveation-labs/sc-module-azureblob-assets-versioning/assets/115815461/27010ab7-85b8-4f53-9d74-0e0e17c5e57a)

A new toolbar has been added as Asset Versions which will give details of previous versions for selected asset. In Previous  version screen use the feature of apply as current version on any previous versions.

![image](https://github.com/reveation-labs/sc-module-azureblob-assets-versioning/assets/115815461/a2b85d3b-dbe9-4996-9642-c897af36fe5d)

## License

Copyright (c) Sharpcode Solutions. All rights reserved.

Licensed under the GNU Affero General Public License v3.0. You may not use this file except in compliance with the License.

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

## Sitecore CDN Accelerator

A Sitecore CDN Accelerator providing the foundation for having Sitecore behind a CDN.

### Framework Features

* Asset Purging on Publish
* MediaProvider implementation to generate CDN Urls based on the target database, using the Media.MediaLinkServerUrl
    * Sitecore currently uses an all or nothing approach to generating CDN Urls

#### Purging Assets

There are two approaches when having Sitecore behind a CDN (not meant to be definitions):

* Load/Push content to the CDN
    * The asset is pushed from the source to the Cache Servers
    * Harder to implement, recommended for web sites with many *larger* assets
* Push/(Reverse Proxy)
    * The CDN acts as a proxy to retrieve the asset from the origin servers, no load is necessary as the asset is retrieved on first request.
    * Easier to implement, recommended for most Websites with many *smaller* assets

The framework currently supports the *Reverse Proxy* approach for assets on the CDN.

##### Purging Framework

The framework adds hooks into the Publishing Process to schedule an Asynchronous Sitecore job that reviews 
the history engine for potential items that need to be purged. This process can be customized by either overriding
the base implementations or adding additional processors to exposed pipelines.

Configured Services that can be overridden:

* **IHistoryService** - Retrieves items within the History Engine
* **IPathService** - Generates the paths that a CDN should purge
* **IDeliveryService** - Responsible for purging from the CDN

Available Pipelines:

* **cdn.purgeFilter** - Used to filter the items that should be passed to the IDeliveryService

##### Available CDN Providers

* Azure CDN - [Github Repo](https://github.com/KKings/Sitecore.Feature.CDN.Azure)

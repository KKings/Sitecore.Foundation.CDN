module.exports = () => {
    const root = "E:\\Projects";
    const sandbox = root + "\\Instances\\cdn.local";

    const config = {
        solutionPath: root + "\\Sitecore.Foundation.CDN",
        website: "http://cdn.local",
        websiteRoot: sandbox + "\\Website",
        sitecoreLibraries: sandbox + "\\Website\\bin",
        licensePath: sandbox + "\\Data\\license.xml",
        solutionName: "Sitecore.Foundation.CDN",
        buildConfiguration: "Debug",
        runCleanBuilds: false
    };

    return config;
}
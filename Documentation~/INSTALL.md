# Installation Instructions
## Installing the Plugin via Git

1. Open your Unity project.
2. In the top menu, go to `Window` > `Package Manager`.
3. Click on the `+` button in the top left corner and select `Add package from git URL...`.
4. Enter the following URL and click `Add`:
    ```
    https://github.com/AdVey-io/GottaAsk-Unity.git
    ```

## Adding the External Dependency Manager via Scoped Registries

1. Open your Unity project.
2. In the top menu, go to `Edit` > `Project Settings`.
3. Select `Package Manager` from the list on the left.
4. In the `Scoped Registries` section, click the `+` button to add a new scoped registry.
5. Enter the following details:
    - **Name**: OpenUPM
    - **URL**: `https://package.openupm.com`
    - **Scope(s)**: `com.google.external-dependency-manager`
6. Click `Save` to add the scoped registry.
7. Go to `Window` > `Package Manager`.
8. Click on the `+` button in the top left corner and select `Add package by name...`.
9. Enter the following package name and click `Add`:
    ```
    com.google.external-dependency-manager
    ```
10. The External Dependency Manager will be added to your project.

That's it! You have successfully installed the plugin and the External Dependency Manager.

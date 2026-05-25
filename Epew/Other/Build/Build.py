from ScriptCollection.TFCPS.DotNet.TFCPS_CodeUnitSpecific_DotNet import TFCPS_CodeUnitSpecific_DotNet_Functions,TFCPS_CodeUnitSpecific_DotNet_CLI
from ScriptCollection.GeneralUtilities import GeneralUtilities, Platform

def build():
    tf:TFCPS_CodeUnitSpecific_DotNet_Functions=TFCPS_CodeUnitSpecific_DotNet_CLI.parse(__file__)
    codeunit_folder=tf.get_codeunit_folder()
    platforms:list[Platform] = [
            Platform.Windows_AMD64,
            Platform.Linux_AMD64,
            Platform.Linux_ARM64,
    ]

    #usual build
    tf.build([GeneralUtilities.platform_to_dotnet_runtime_identifier(p) for p in platforms], False)

    #deb-file
    tf.tfcps_Tools_General.create_deb_package_for_artifact(codeunit_folder, tf.tfcps_Tools_General.get_constant_value(codeunit_folder, "MaintainerName"),tf.tfcps_Tools_General.get_constant_value(codeunit_folder, "MaintainerEMailAddress"), tf.tfcps_Tools_General.get_constant_value(codeunit_folder, "CodeUnitDescription"))
    
    #winget-manifest
    winget_artifact_name: str = "Epew-Zip-for-Windows"
    tf.tfcps_Tools_General.create_zip_file_for_artifact(codeunit_folder, "BuildResult_DotNet_win-x64", winget_artifact_name)
    tf.tfcps_Tools_General.generate_winget_zip_manifest(codeunit_folder, winget_artifact_name)


if __name__ == "__main__":
    build()

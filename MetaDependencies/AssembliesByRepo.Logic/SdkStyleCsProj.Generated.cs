﻿namespace AssInfo.Logic.Space
{
    // The child classes in this partial implementation were generated by copying XML
    // and using Paste Special / Paste XML as Classes.

    public partial class SdkStyleCsProj
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Project
        {

            private ProjectPropertyGroup propertyGroupField;

            private string sdkField;

            /// <remarks/>
            public ProjectPropertyGroup PropertyGroup
            {
                get
                {
                    return this.propertyGroupField;
                }
                set
                {
                    this.propertyGroupField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Sdk
            {
                get
                {
                    return this.sdkField;
                }
                set
                {
                    this.sdkField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ProjectPropertyGroup
        {

            private string targetFrameworkField;

            private string targetFrameworksField;

            private string assemblyNameField;

            private string rootNamespaceField;

            private bool generatePackageOnBuildField;

            private bool publishPackageField;

            private string packageIdField;

            private string repositoryUrlField;

            private string descriptionField;

            /// <remarks/>
            public string TargetFramework
            {
                get
                {
                    return this.targetFrameworkField;
                }
                set
                {
                    this.targetFrameworkField = value;
                }
            }

            /// <remarks/>
            public string TargetFrameworks
            {
                get
                {
                    return this.targetFrameworksField;
                }
                set
                {
                    this.targetFrameworksField = value;
                }
            }

            /// <remarks/>
            public string AssemblyName
            {
                get
                {
                    return this.assemblyNameField;
                }
                set
                {
                    this.assemblyNameField = value;
                }
            }

            /// <remarks/>
            public string RootNamespace
            {
                get
                {
                    return this.rootNamespaceField;
                }
                set
                {
                    this.rootNamespaceField = value;
                }
            }

            /// <remarks/>
            public bool GeneratePackageOnBuild
            {
                get
                {
                    return this.generatePackageOnBuildField;
                }
                set
                {
                    this.generatePackageOnBuildField = value;
                }
            }

            /// <remarks/>
            public bool PublishPackage
            {
                get
                {
                    return this.publishPackageField;
                }
                set
                {
                    this.publishPackageField = value;
                }
            }

            /// <remarks/>
            public string PackageId
            {
                get
                {
                    return this.packageIdField;
                }
                set
                {
                    this.packageIdField = value;
                }
            }

            /// <remarks/>
            public string RepositoryUrl
            {
                get
                {
                    return this.repositoryUrlField;
                }
                set
                {
                    this.repositoryUrlField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }
        }

    }
}

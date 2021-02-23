﻿namespace MergeGraphs.Logic
{
    /// <summary>
    /// Represents a directed graph in a way that is compatible with DGML notation.
    /// </summary>
    public partial class Dgml
    {
        // This code has been generated by copying DependencetSample.dgml XML contents and using Edit / Paste Special / Paste XML as Classes.


        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/vs/2009/dgml", IsNullable = false)]
        public partial class DirectedGraph
        {

            private DirectedGraphNode[] nodesField;

            private DirectedGraphLink[] linksField;

            private DirectedGraphCategory[] categoriesField;

            private DirectedGraphStyle[] stylesField;

            private DirectedGraphProperty[] propertiesField;

            private string graphDirectionField;

            private string layoutField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Node", IsNullable = false)]
            public DirectedGraphNode[] Nodes
            {
                get
                {
                    return this.nodesField;
                }
                set
                {
                    this.nodesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Link", IsNullable = false)]
            public DirectedGraphLink[] Links
            {
                get
                {
                    return this.linksField;
                }
                set
                {
                    this.linksField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Category", IsNullable = false)]
            public DirectedGraphCategory[] Categories
            {
                get
                {
                    return this.categoriesField;
                }
                set
                {
                    this.categoriesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Style", IsNullable = false)]
            public DirectedGraphStyle[] Styles
            {
                get
                {
                    return this.stylesField;
                }
                set
                {
                    this.stylesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Property", IsNullable = false)]
            public DirectedGraphProperty[] Properties
            {
                get
                {
                    return this.propertiesField;
                }
                set
                {
                    this.propertiesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string GraphDirection
            {
                get
                {
                    return this.graphDirectionField;
                }
                set
                {
                    this.graphDirectionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Layout
            {
                get
                {
                    return this.layoutField;
                }
                set
                {
                    this.layoutField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphNode
        {

            private string idField;

            private string labelField;

            private string categoryField;

            private string iconField;

            private string processorArchitectureField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Label
            {
                get
                {
                    return this.labelField;
                }
                set
                {
                    this.labelField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Category
            {
                get
                {
                    return this.categoryField;
                }
                set
                {
                    this.categoryField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Icon
            {
                get
                {
                    return this.iconField;
                }
                set
                {
                    this.iconField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string ProcessorArchitecture
            {
                get
                {
                    return this.processorArchitectureField;
                }
                set
                {
                    this.processorArchitectureField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphLink
        {

            private string sourceField;

            private string targetField;

            private string labelField;

            private string sourceNodeDetailsField;

            private string targetNodeDetailsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Source
            {
                get
                {
                    return this.sourceField;
                }
                set
                {
                    this.sourceField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Target
            {
                get
                {
                    return this.targetField;
                }
                set
                {
                    this.targetField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Label
            {
                get
                {
                    return this.labelField;
                }
                set
                {
                    this.labelField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string SourceNodeDetails
            {
                get
                {
                    return this.sourceNodeDetailsField;
                }
                set
                {
                    this.sourceNodeDetailsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TargetNodeDetails
            {
                get
                {
                    return this.targetNodeDetailsField;
                }
                set
                {
                    this.targetNodeDetailsField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphCategory
        {

            private string idField;

            private string labelField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Label
            {
                get
                {
                    return this.labelField;
                }
                set
                {
                    this.labelField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphStyle
        {

            private DirectedGraphStyleCondition conditionField;

            private DirectedGraphStyleSetter[] setterField;

            private string targetTypeField;

            private string groupLabelField;

            /// <remarks/>
            public DirectedGraphStyleCondition Condition
            {
                get
                {
                    return this.conditionField;
                }
                set
                {
                    this.conditionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Setter")]
            public DirectedGraphStyleSetter[] Setter
            {
                get
                {
                    return this.setterField;
                }
                set
                {
                    this.setterField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string TargetType
            {
                get
                {
                    return this.targetTypeField;
                }
                set
                {
                    this.targetTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string GroupLabel
            {
                get
                {
                    return this.groupLabelField;
                }
                set
                {
                    this.groupLabelField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphStyleCondition
        {

            private string expressionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Expression
            {
                get
                {
                    return this.expressionField;
                }
                set
                {
                    this.expressionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphStyleSetter
        {

            private string propertyField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Property
            {
                get
                {
                    return this.propertyField;
                }
                set
                {
                    this.propertyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/vs/2009/dgml")]
        public partial class DirectedGraphProperty
        {

            private string idField;

            private string dataTypeField;

            private string labelField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DataType
            {
                get
                {
                    return this.dataTypeField;
                }
                set
                {
                    this.dataTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Label
            {
                get
                {
                    return this.labelField;
                }
                set
                {
                    this.labelField = value;
                }
            }
        }

    }
}
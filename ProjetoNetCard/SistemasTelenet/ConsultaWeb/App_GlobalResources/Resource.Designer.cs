//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "11.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Resource", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo CPF é obrigatório..
        /// </summary>
        internal static string ErroCPFObrigatorio {
            get {
                return ResourceManager.GetString("ErroCPFObrigatorio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digite uma data final válida..
        /// </summary>
        internal static string ErroDataFinalInvalida {
            get {
                return ResourceManager.GetString("ErroDataFinalInvalida", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data final deverá ser maior que data inicial..
        /// </summary>
        internal static string ErroDataFinalMenor {
            get {
                return ResourceManager.GetString("ErroDataFinalMenor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digite uma data inicial válida..
        /// </summary>
        internal static string ErroDataInicialInvalida {
            get {
                return ResourceManager.GetString("ErroDataInicialInvalida", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data pesquisada não é uma data válida..
        /// </summary>
        internal static string ErroDataInvalida {
            get {
                return ResourceManager.GetString("ErroDataInvalida", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data pesquisada não deverá ultrapassar 90 dias..
        /// </summary>
        internal static string ErroDataMaior3Meses {
            get {
                return ResourceManager.GetString("ErroDataMaior3Meses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A data pesquisada não deverá ultrapassar 90 dias..
        /// </summary>
        internal static string ErroIntervaloData {
            get {
                return ResourceManager.GetString("ErroIntervaloData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} precisa ser uma data..
        /// </summary>
        internal static string FieldMustBeDate {
            get {
                return ResourceManager.GetString("FieldMustBeDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} precisa ser um número..
        /// </summary>
        internal static string FieldMustBeNumeric {
            get {
                return ResourceManager.GetString("FieldMustBeNumeric", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O valor &apos;{0}&apos; não é um valor valido para {1}..
        /// </summary>
        internal static string PropertyValueInvalid {
            get {
                return ResourceManager.GetString("PropertyValueInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O campo {0} é obrigatório..
        /// </summary>
        internal static string PropertyValueRequired {
            get {
                return ResourceManager.GetString("PropertyValueRequired", resourceCulture);
            }
        }
    }
}
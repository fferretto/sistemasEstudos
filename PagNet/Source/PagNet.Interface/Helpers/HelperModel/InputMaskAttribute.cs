using System;

namespace PagNet.Interface.Helpers.HelperModel
{
    public class InputMaskAttribute : Attribute//, IMetadataAware
    {

        public string Mask { get; set; }
        public string Url { get; set; }
        public bool IsReverso { get; set; }

        public InputMaskAttribute(string mask)
        {
            Mask = mask;
            IsReverso = false;
        }

    }
}
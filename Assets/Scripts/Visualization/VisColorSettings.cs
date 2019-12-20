using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Visualization
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Visualization Colors", menuName = "ScriptableObjects/VisColorSettings", order = 1)]
    public class VisColorSettings : ScriptableObject
    {
        public enum ColorMeaning
        {
            PositiveElectric,
            NegativeElectric,
            PositiveMagnetic,
            NegativeMagnetic,
            Neutral,
            Error
        }
        
        [SerializeField]
        private Color positiveElectricColor;
        [SerializeField]
        private Color negativeElectricColor;
        [SerializeField]
        private Color positiveMagneticColor;
        [SerializeField]
        private Color negativeMagneticColor;
        [SerializeField]
        private Color neutralColor;
        [SerializeField]
        private Color errorColor;

        //private Dictionary<ColorMeaning, Color> colorDict = new Dictionary<ColorMeaning, Color>();
        private Dictionary<ColorMeaning, Material> materialDict = new Dictionary<ColorMeaning, Material>();
        
        [SerializeField]
        private Material baseMaterial;
        
//        //[HideInInspector]
//        public Material positiveElectricMaterial;
//        //[HideInInspector]
//        public Material negativeElectricMaterial;
//        [HideInInspector]
//        public Material positiveMagneticMaterial;
//        [HideInInspector]
//        public Material negativeMagneticMaterial;
//        [HideInInspector]
//        public Material neutralMaterial;
//        [HideInInspector]
//        public Material errorMaterial;

        public Color GetColor(ColorMeaning meaning)
        {
            switch (meaning)
            {
                case ColorMeaning.PositiveElectric: return positiveElectricColor;
                case ColorMeaning.NegativeElectric: return negativeElectricColor;
                case ColorMeaning.PositiveMagnetic: return positiveMagneticColor;
                case ColorMeaning.NegativeMagnetic: return negativeMagneticColor;
                case ColorMeaning.Neutral: return neutralColor;
                case ColorMeaning.Error: return errorColor;
                default: return errorColor;
            }
        }
        public Material GetMaterial(ColorMeaning meaning)
        {
            if (!materialDict.ContainsKey(meaning) || !materialDict[meaning])
            {
                materialDict[meaning] = Instantiate(baseMaterial);
                materialDict[meaning].color = GetColor(meaning);
            }
            return materialDict[meaning];
        }
        
//        public Material PositiveElectricMaterial
//        {
//            get
//            {
//                if (!positiveElectricMaterial)
//                {
//                    positiveElectricMaterial = Instantiate(baseMaterial);
//                    positiveElectricMaterial.color = positiveElectricColor;
//                }
//
//                return positiveElectricMaterial;
//            }
//        }
//        public Material NegativeElectricMaterial
//        {
//            get
//            {
//                if (!negativeElectricMaterial)
//                {
//                    negativeElectricMaterial = Instantiate(baseMaterial);
//                    negativeElectricMaterial.color = negativeElectricColor;
//                }
//
//                return negativeElectricMaterial;
//            }
//        }
    }
}
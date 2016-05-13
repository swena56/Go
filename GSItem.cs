using UnityEngine;
using UnityEngine.UI;
namespace GameSlyce
{
    public class GSItem
    {
        public Text nameTxt;
       
        protected void SetValues(string nameString)
        {
            nameTxt.text = nameString;
           // tglBtn.isOn = isChecked;

        }
       
    }
}
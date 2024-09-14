using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;

namespace RevitAPICourse
{
    [Transaction(TransactionMode.Manual)]

    internal class Selection : IExternalCommand
    {
        UIApplication _uiapp;
        Application _app;
        UIDocument _uidoc;
        Document _doc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //throw new NotImplementedException();

            //Creating App and doc objects.
            _uiapp = commandData.Application;
            _app = _uiapp.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _doc = _uidoc.Document;

            //ShowElementInfo(PickMethod_PickObject());
            ShowElementListIndo(PickMethod_PickObjects());

            return Result.Succeeded;
        }

        #region PickMethod_PickObject()

        /// <summary>
        /// Prompt the user to select an element and retrieve this element
        /// </summary>

        public Element PickMethod_PickObject()
        {
            //criando um elemento do tipo "referência", onde o valor da variável "Reference" está sendo o retorno de um método que me permite selecionar algo
            Reference r = _uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select an element");

            Element e = _uidoc.Document.GetElement(r);

            return e;
        }
        #endregion

        #region ShowElementInfo()

        /// <summary>
        /// Show element information
        /// </summary>
        /// <param name="e">Picked element</param>

        //criando o método "ShowElementInfo" que está recebendo como parâmetro outro elemento que está vindo de outro método    
        public void ShowElementInfo(Element e) 
        {
            //resgatando o nome desse elemento e armazenando dentro da variável "s"
            string s = e.Name;

            //exibindo isso
            TaskDialog.Show("Element information", $"Element name: {s}");
        }

        #endregion

        #region PickMethod_PickObjects()

        /// <summary>
        /// Prompt the user to select multiple elements
        /// </summary>
        /// <returns>Element list</returns>

        //criando método para seleção de múltiplos elementos

        public IList<Element> PickMethod_PickObjects()
        {
            IList<Reference> refs = _uidoc.Selection.PickObjects
                (Autodesk.Revit.UI.Selection.ObjectType.Element, "Select multiple elements");

            //criando uma lista do tipo "Element", chamando ela de "elements" e instanciando
            IList<Element> elements = new List<Element>();

            foreach (Reference r in refs)
            {
                Element e = _uidoc.Document.GetElement (r);

                elements.Add(e);
            }

            return elements;

        }

        #endregion

        #region ShowElementListInfo()

        /// <summary>
        /// Show a list of element name
        /// </summary>
        /// <param name="elements">Element list</param>

        public void ShowElementListIndo(IList<Element> elements)
        {
            string s = string.Empty;

            foreach (Element e in elements)
            {
                s += $"{ e.Name}\n";
            }

            TaskDialog.Show("Element information", $"Element name: {s}");
        }

        #endregion
    }
}

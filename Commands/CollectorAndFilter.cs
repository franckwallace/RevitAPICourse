using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//adicionando os namespaces
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;

namespace RevitAPICourse
{
    [Transaction(TransactionMode.ReadOnly)]

    //extendendo a classe e implementando sua interface
    internal class CollectorAndFilter : IExternalCommand
    {
        //criando variáveis globais (ficam aqui para poder acessá-las a partir de qualquer método que estiver dentro dessa classe)
        UIApplication _uiapp;
        Application _app;
        UIDocument _uidoc;
        Document _doc;

        //Execute é o método que será chamado quando o plug-in for chamado
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //comentando para mostrar que isso será descartado
            //throw new NotImplementedException();

            //Creating App and doc objects.
            //atribuindo os valores das variáveis
            _uiapp = commandData.Application;
            _app = _uiapp.Application;
            _uidoc = _uiapp.ActiveUIDocument;
            _doc = _uidoc.Document;

            //adicionando o método "CountDoors" (lá embaixo) e passando o "FilterDoors" como argumento desse método
            CountElement(FilterWalls());

            //retornando o método 
            return Result.Succeeded;

        }

        #region FilterDoors ()
        /// <summary>
        /// Filter doors in the model.
        /// </summary>
        /// <returns>filtered doors.</returns>

        //criando um método de filtrar portas
        public IList<Element> FilterDoors()
        {
            //criando um coletor (dentro do documento em questão, _doc)
            FilteredElementCollector collector = new FilteredElementCollector(_doc);

            //criando o filtro (de portas, no caso)
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

            //unindo o coletor com o filtro, fazendo um cast para elementos
            IList<Element> filteredDoors = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            return filteredDoors;
        }

        #endregion

        #region FilterWalls()
        /// <summary>
        /// Filtered walls.
        /// </summary>
        /// <returns>filtered walls.</returns>

        public IList<Element> FilterWalls()
        {
            ElementId viewId = _doc.ActiveView.Id;

            //criando um coletor
            FilteredElementCollector collector = new FilteredElementCollector(_doc, viewId);

            //criando um filtro (wall)
            ElementClassFilter filter = new ElementClassFilter(typeof(Wall));

            //Especificando/adicionando parâmetros ao filtro
            IList<Element> filteredWalls = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            return filteredWalls;
        }



        #endregion


        #region CountElement()
        /// <summary>
        /// Counts the elements.
        /// </summary>
        /// <param name="filteredElements">Elements amount</param>

        //criando um método de contar portas
        public void CountElement(IList<Element> filteredElements)
        {
            TaskDialog.Show("Elements", $"Total elements: {filteredElements.Count}");
        }

        #endregion

    }
}

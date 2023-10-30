namespace FlatLands.UI.Examples
{
    public sealed class ExampleWindow : UIWindow
    {
        private ExampleWindowModel _model;
        
        internal override void SetModel(IUIModel model)
        {
            _model = model as ExampleWindowModel;
        }
    }
}
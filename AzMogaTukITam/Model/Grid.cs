namespace AzMogaTukITam.Model
{
    public class Grid
    {

        public Grid (int h, int w)
        {
            this.Height = h;
            this.Width = w;
        }

        public int Height { get; set; } = 6;
        public int Width { get; set; } = 8;

        public List<LayerBase> Layers { get; set; } = new List<LayerBase>();

        public DisplayValue[,] ConstructGrid()
        {
            DisplayValue[,] finalGrid = new DisplayValue[this.Height, this.Width];
            foreach (LayerBase layer in this.Layers.OrderBy(x => x.ZIndex).ThenBy(x => x.LayerID))
                for (int y = 0; y < layer.Data.GetLength(0); y++)
                    for (int x = 0; x < layer.Data.GetLength(1); x++)
                        if (layer.Data[y, x]) finalGrid[y, x] = layer.DisplayValue;
            return finalGrid;
        }

    }
}

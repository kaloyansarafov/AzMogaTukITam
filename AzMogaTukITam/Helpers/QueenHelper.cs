using AzMogaTukITam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzMogaTukITam.Helpers
{
    public static class QueenHelper
    {

        public static bool CanPlaceQueen(int row, int col, string layerId, Grid grid)
        {
            if (grid.Layers.Where(x => x is PlayerLayer && x.LayerID != layerId) is IEnumerable<PlayerLayer>
                playerLayers)
                foreach (var unused in playerLayers)
                {
                    

                        return false;
                }

            return true;
        }

    }
}

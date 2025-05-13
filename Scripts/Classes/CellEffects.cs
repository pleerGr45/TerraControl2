namespace TC_basic {

    
    public class CellEffect {
            
        private CellEffectType effect_type;
        private Cell parent;
        private float duration;

        public CellEffect(Cell parent, CellEffectType effect_type, float duration) {
            this.effect_type = effect_type;
            this.duration = duration;
            this.parent = parent;
        }
            
        public void RealeseEffect(Cell cell) {
            
        }
    }

    public abstract class CellEffectType { 
        private string effect_name;
        
        public CellEffectType(string effect_name) {
            this.effect_name = effect_name;
        }

        public abstract void ApplyEffect(Cell cell);
    }

    public class FireCellEffect : CellEffectType
    {
        public FireCellEffect() : base("fire")
        {

        }

        public override void ApplyEffect(Cell cell)
        {
            
        }

        
    }



}
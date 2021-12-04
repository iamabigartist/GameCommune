namespace MMOBALibrary.Rule
{
    public class HeroActionRule
    {
        public int move_debuffs;
        public bool can_move => move_debuffs == 0;
        public int spell_debuffs;
        public bool can_spell => spell_debuffs == 0;
        public int summon_debuffs;
        public bool can_summon => summon_debuffs == 0;
    }
}

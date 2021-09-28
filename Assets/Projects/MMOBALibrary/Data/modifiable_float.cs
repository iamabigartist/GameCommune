namespace MMOBALibrary.Data
{
    /// <summary>
    ///     A practical float data type that allow to be fine-grained edited and will provide a final <see cref="value" />
    ///     after modification.
    /// </summary>
    public class modifiable_float
    {
        public modifiable_float(float basic_value, float mul_bonus_value, float add_bonus_value)
        {
            _basic = basic_value;
            _mul_bonus = mul_bonus_value;
            _add_bonus = add_bonus_value;
            refresh_value();
        }

        float _basic, _mul_bonus, _add_bonus;

        /// <summary>
        ///     The final calculated stage
        /// </summary>
        public float value
        {
            get;
            private set;
        }

        void refresh_value()
        {
            value = basic +
                    mul_bonus * basic +
                    add_bonus;
        }

        /// <summary>
        ///     The basic stage of this data
        /// </summary>
        public float basic
        {
            get => _basic;
            set
            {
                _basic = value;
                refresh_value();
            }
        }

        /// <summary>
        ///     The bonus stage based on a ratio of the basic stage
        /// </summary>
        public float mul_bonus
        {
            get => _mul_bonus;
            set
            {
                _mul_bonus = value;
                refresh_value();
            }
        }

        /// <summary>
        ///     The bonus stage added
        /// </summary>
        public float add_bonus
        {
            get => _add_bonus;
            set
            {
                _add_bonus = value;
                refresh_value();
            }
        }



    }
}

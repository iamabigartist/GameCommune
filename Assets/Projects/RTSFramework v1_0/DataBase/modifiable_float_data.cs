namespace RTSFramework_v1_0.DataBase
{
    /// <summary>
    ///     A practical float data type that allow to be fine-grained edited and will provide a final <see cref="value" />
    ///     after modification.
    /// </summary>
    public class modifiable_float_data
    {
        public modifiable_float_data(float basic_value, float mul_bonus_value, float add_bonus_value)
        {
            _basic = new PrimitiveField<float>( basic_value );
            _mul_bonus = new PrimitiveField<float>( mul_bonus_value );
            _add_bonus = new PrimitiveField<float>( add_bonus_value );
            refresh_value();
        }

        PrimitiveField<float> _basic, _mul_bonus, _add_bonus;

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
            value = basic.value +
                    mul_bonus.value * basic.value +
                    add_bonus.value;
        }

        /// <summary>
        ///     The basic stage of this data
        /// </summary>
        public PrimitiveField<float> basic
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
        public PrimitiveField<float> mul_bonus
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
        public PrimitiveField<float> add_bonus
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

namespace RTSFramework_v0_2.src.Data
{
    /// <summary>
    ///     A practical float data type that allow to be fine-grained edited and will provide a final <see cref="value" />
    ///     after modification.
    /// </summary>
    public class modified_float_data
    {
        public modified_float_data(float basic_value, float mul_bonus_value, float add_bonus_value)
        {
            _basic = new float_data( basic_value );
            _mul_bonus = new float_data( mul_bonus_value );
            _add_bonus = new float_data( add_bonus_value );
            refresh_value();
        }

        float_data _basic, _mul_bonus, _add_bonus;

        /// <summary>
        ///     The final calculated value
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
        ///     The basic value of this data
        /// </summary>
        public float_data basic
        {
            get => _basic;
            set
            {
                _basic = value;
                refresh_value();
            }
        }

        /// <summary>
        ///     The bonus value based on a ratio of the basic value
        /// </summary>
        public float_data mul_bonus
        {
            get => _mul_bonus;
            set
            {
                _mul_bonus = value;
                refresh_value();
            }
        }

        /// <summary>
        ///     The bonus value added
        /// </summary>
        public float_data add_bonus
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

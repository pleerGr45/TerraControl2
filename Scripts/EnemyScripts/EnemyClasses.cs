using System;

namespace TC_enemy {

    public class LevelWaves {
        
        /* Infinity waves, wave instruction repeated */
        private bool is_infinity;
        /* Wave count
           If wave count ( > 0 ) */
        private int wave_count; 
        private Wave[] waves;
        public int GetWaveCount { get => wave_count; }

        public void SetWaveCount(int wave_count) {
            int old_wave_count = this.wave_count;
            this.wave_count = wave_count > 0 ? wave_count : 1;

            Wave[] old_waves = waves;
            waves = new Wave[wave_count];

            for (int i = 0; i < wave_count; i++)
                waves[i] = i < old_wave_count ? old_waves[i] : new Wave();
            
        }

        public void ChangeWave(int index, byte status = 5, float wave_time = -1) {
            if (index < 0 || index >= wave_count)
                return;
            
            if (status >= 5) {
                status = waves[index].Status;
                wave_time = waves[index].WaveTime;
            }

            if (wave_time <= 0) {
                wave_time = waves[index].WaveTime;
            }

        }

        public Wave GetWave(int index) {
            return waves[index % wave_count];
        }


        public float GetTotalTime() { 
            return is_infinity ? -1 : 0;
        }
        

        public LevelWaves(int wave_count) {
            SetWaveCount(wave_count);
        }

        /* !!!!!!!!!!!!! ПЕРЕДЕЛАТЬ */
        public override string ToString()
        {
            return base.ToString();
        }
        
    }

    public class Wave {

        /* Wave status 
                0 - safe (no enemies) (white)
                1 - easy (green)
                2 - average (yellow)
                3 - hard (red)
                4 - impossible (violet)
        */
        private byte status;

        private float wave_time;

        public byte Status { get => status; set => status = (byte)(value > 4 ? 4 : (value < 0 ? 0 : value)); }
        public float WaveTime { get => wave_time; set => wave_time = value; }

        public Wave() {

        }

    }
}
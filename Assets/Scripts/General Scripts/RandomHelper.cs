using System;

public class RandomHelper {
    private static Random random = new Random();

    public static double Uniform(double low, double high){
        double amount = random.NextDouble();
        return low + (high - low) * amount;
    }

    public static float Uniform(float low, float high){
        float amount = (float)random.NextDouble();
        return low + (high - low) * amount;
    }
}

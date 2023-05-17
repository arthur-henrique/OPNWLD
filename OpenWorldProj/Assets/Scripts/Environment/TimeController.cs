using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Profiling;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private DateTime currentTime;

    // Sun
    [SerializeField] private Light sunLight;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;

    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    // Colors and Ambient Light
    [SerializeField] private AnimationCurve lightChangeCurve;
    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private float maxSunIntensity;

    // Moon
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonIntensity;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        if(timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }
    private void RotateSun()
    {
        float sunLightRotation;
        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(dayAmbientLight, nightAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan diff = toTime - fromTime;
        if(diff.TotalSeconds < 0) 
        {
            diff += TimeSpan.FromHours(24);
        }
        return diff;
    }
}

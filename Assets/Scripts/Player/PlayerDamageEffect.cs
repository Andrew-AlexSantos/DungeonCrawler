using System.Collections;
using System.Collections.Generic;
using EventsArgs;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDamageEffect : MonoBehaviour {

    public Volume volume;
    public LifeScript life;
    public float minWeight = 0.4f;
    public float maxWeight = 1.0f;

    private void Start() {
        life.OnDamage += OnDamage;
    }

    private void Update()   {
        float alpha = Time.deltaTime / 1f;
        float newWeight = Mathf.Lerp(volume.weight, 0f, alpha);
        volume.weight = newWeight;

    }

    private void OnDamage(object sender, DamageEventArgs e) {
        float lifeRate = (float) life.health / (float) life.maxHealth;
        float effectIntensity = minWeight + (maxWeight - minWeight) * (1f - lifeRate);
        volume.weight = effectIntensity;
    }


}

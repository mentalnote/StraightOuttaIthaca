﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PrayerTracker : MonoBehaviour
{
    [System.Serializable]
    public class TitleEntry
    {
        public string Title;
        public int FollowersRequirement;
        public AudioClip SoundEffect;
    }

    [System.Serializable]
    public class SpellEntry
    {
        public Button SpellButton;
        public int FollowersRequirement;
    }

    public int FollowerCount
    {
        get { return _followerCount; }
        set
        {
            _followerCount = value;
            if (_spellDictionary.ContainsKey(_followerCount))
            {
                for (int i = 0; i < _spellDictionary[_followerCount].Count; i++)
                {
                    _spellDictionary[_followerCount][i].interactable = true;
                }
            }
            if (_titleDictionary.ContainsKey(_followerCount) && _titleText != null)
            {
                _titleText.text = _titleDictionary[_followerCount];
            }
            if (_soundEffectDictionary.ContainsKey(_followerCount))
            {
                AudioSource.PlayClipAtPoint(_soundEffectDictionary[_followerCount], Vector3.zero);
                _soundEffectDictionary.Remove(_followerCount);
            }
        }
    }

    [SerializeField] private SpellEntry[] _spells;
    [SerializeField] private Text _titleText;
    [SerializeField] private TitleEntry[] _titles;

    private int _followerCount;
    private Dictionary<int, List<Button>> _spellDictionary; 
    private Dictionary<int, string> _titleDictionary;
    private Dictionary<int, AudioClip> _soundEffectDictionary;

	// Use this for initialization
    private void Start()
    {
        _spellDictionary = new Dictionary<int, List<Button>>();
        _titleDictionary = new Dictionary<int, string>();
        _soundEffectDictionary = new Dictionary<int, AudioClip>();
        for (int i = 0; i < _spells.Length; i++)
        {
            if (!_spellDictionary.ContainsKey(_spells[i].FollowersRequirement))
            {
                _spellDictionary[_spells[i].FollowersRequirement] = new List<Button>();
            }
            _spellDictionary[_spells[i].FollowersRequirement].Add(_spells[i].SpellButton);
        }
        for (int i = 0; i < _titles.Length; i++)
        {
            _titleDictionary[_titles[i].FollowersRequirement] = _titles[i].Title;
            _soundEffectDictionary[_titles[i].FollowersRequirement] = _titles[i].SoundEffect;
        }
    FollowerCount = _followerCount;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

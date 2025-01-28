using UnityEngine;
using System;


/// <summary>
/// �v���C���[�̐i�s�ɉ����Ēn�`�𐶐�����N���X
/// </summary>
public class Generator : MonoBehaviour {
    [SerializeField] private GameObject[] _ground = new GameObject[3]; // �n�`�v���n�u�̃��X�g
    private float _generatorPositionX = 0;    // ���݂̐����ʒu
    private int _generateLimit = 100;          // �n�`�����̏���ʒu
    private GameObject _player;                // �v���C���[�I�u�W�F�N�g
    private int _randomPool;                   // �����_���ɑI�΂��C���f�b�N�X
    private  const int GENERATE_RANGE = 50;           // ������ǉ�����͈�
    private const int GENERATE_ADD = 100;            // �����͈͂��g������l

    void Start() {
        // �v���C���[�I�u�W�F�N�g���擾
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        // �K�v�ɉ����Ēn�`�𐶐����鏈�����Ăяo��
        GenerateGroundIfNeeded();
    }

    /// <summary>
    /// �K�v�ɉ����Ēn�`�𐶐����鏈��
    /// </summary>
    private void GenerateGroundIfNeeded() {
        // �����_���V�[�h�����݂̎����ŏ�����
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        // �����ʒu������ɒB���Ă��Ȃ��ꍇ�A�n�`�𐶐�����
        while (_generatorPositionX < _generateLimit) {
            // �����_���ɐ�������n�`��I��
            _randomPool = UnityEngine.Random.Range(0, _ground.Length);

            // ���S�Ȑ����͈̓`�F�b�N
            if (_randomPool >= 0 && _randomPool < _ground.Length) {
                // �n�`���w��̈ʒu�ɐ���
                GameObject ground = Instantiate(
                    _ground[_randomPool],
                    new Vector2(transform.position.x + _generatorPositionX, 0f),
                    Quaternion.identity
                );

                // ���������n�`�̃T�C�Y���擾���A�����ʒu���X�V
                _generatorPositionX += ground.GetComponent<GroundSize>()._groundSize;
            }
        }

        // �v���C���[���i�s�����ʒu�ɉ����Đ����͈͂��g��
        if (_generateLimit - _player.transform.position.x < GENERATE_RANGE) {
            _generateLimit += GENERATE_ADD;
        }
    }
}

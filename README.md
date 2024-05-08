# avatar-control
Unity上でアバターのちょっとしたパラメータとかを動かしたりするモジュール置き場です

## BlendShapeController.cs
任意のブレンドシェイプの変化に連動して、別のブレンドシェイプを揺らすやつです

### 使い方
任意のオブジェクトにアタッチして、monitored blendshape（監視するブレンドシェイプ）とtargetBlendshape（連動させるブレンドシェイプ）を指定
パラメータは以下

#### skinnedMeshRenderer
ブレンドシェイプを持つメッシュレンダラを指定します。VRoidの表情とかはFace、VRCアバターだとBodyが多いです。

#### multiplier
監視ブレンドシェイプに対するターゲットの倍率です。大きいほどグワングワン動きます

#### springStrength
振り子物理演算の強さです

### damping
物理演算の減衰です


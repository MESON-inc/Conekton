# Getting Started for Multiplayer

以下で説明する機能たちはExtenjectのインストーラを適切に設定することで利用可能になります。
説明に記載されているインストーラを設定して利用ください。

また、マルチプレイの機能は[ARUtilityの機能](./GettingStarted-ARUtility.md)も利用しているため、そちらの機能も適切にセットアップする必要があります。



## Avatar System

namespace: `Conekton.ARMultiplayer.Avatar.Domain`

アバターシステムは、AR/VRデバイスの頭の位置や手、コントローラの位置を視覚化するための機能です。
マルチプレイ時にはリモートプレイヤーとして、接続先の相手の頭や手の位置を視覚化する目的でも利用されます。（つまりオンライン限定の機能ではなく、オフラインでも利用可能です）

アバターシステムに利用されるオブジェクトは「`Assets/Conekton/Multiplayer/Prefabs/Avatar`」にあるPrefabを直接修正するか、`AvatarInstaller`に設定されているPrefabの参照を差し替えることで変更することができます。

システムのインターフェースは以下の通りです。

```
public interface IAvatarService
{
    IAvatar Create();
    void Remove(AvatarID id);
    IAvatar Find(AvatarID id);
    IAvatar GetMain();
}
```

### アバターの生成

アバターは前述のPrefabを元に生成されます。生成したい場合は`Create`を実行することで`IAvatar`インターフェースを持つオブジェクトが生成されます。

メインとなるアバターはシステムが自動で生成するため特に対応する必要はありません。
メインとなるアバターの参照が必要な場合は`GetMain()`を介してアクセスすることができます。

自身で生成した場合など、アバターIDから参照を得たい場合は`Find(AvatarID id)`で参照を得ることができます。


### Installer

`IAvatarSystem`の機能を利用するためには`AvatarInstaller`を利用ください。


## IAvatar

アバターシステムによって生成されたオブジェクトは`IAvatar`インターフェースを持つオブジェクトになります。`IAvatar`インターフェースは以下の通りです。

主に`Transform`などアバターオブジェクトの状態を得るために用います。

```
public interface IAvatar
{
    AvatarID AvatarID { get; }
    IAvatarController AvatarController { get; }
    Transform Root { get; }

    void SetAvatarController(IAvatarController controller);
    void SetWearablePack(IAvatarWearablePack pack);
    void SetWearable(IAvatarWearable wearable);

    Transform GetTransform(AvatarPoseType type);
    Pose GetPose(AvatarPoseType type);
    Pose GetLocalPose(AvatarPoseType type);
    void Destory();
}
```

## IAvatarController

`IAvatar`は`IAvatarController`インターフェースを持つオブジェクトによって操作されます。（後述するネットワーク同期ではリモートプレイヤーの位置同期に用いられます）

```
public interface IAvatarController
{
    Pose GetHeadPose();
    Pose GetHandPose(AvatarPoseType type);
}
```

## IAvatarWearable

アバターシステムには、頭や手の位置にオブジェクトを配置する、ゲームの装備のようなシステムが用意されています。それが`IAvatarWearable`です。

このインターフェースを持つオブジェクトをアバターに渡すことで、対象の位置にオブジェクトを配置することができます。

```
public interface IAvatarWearable
{
    AvatarWearType WearType { get; }
    void TargetTransform(Transform trans);
    void Unwear();
}
```

`AvatarWearType`は以下のように定義されています。それぞれ、頭、左手、右手に対してオブジェクトを配置することができます。

```
public enum AvatarWearType
{
    None,
    Head,
    Left,
    Right,
}
```

### IAvatarWearablePackによるセット

`IAvatarWearable`はひとつのオブジェクトを表します。
これを、まとめてセットとして扱いたい場合のために`IAvatarWearablePack`があります。

このインターフェースを実装したオブジェクトをアバターに渡すことで、任意の数のWearableオブジェクトを設定することができます。

```
public interface IAvatarWearablePack
{
    IAvatarWearable[] GetWearables();
}
```


## Multiplayer Network System

namespace: `Conekton.ARMultiplayer.NetworkMultiplayer.Domain`

マルチプレイヤーネットワークシステムは、マルチプレイヤーのための機能を提供します。

システムのインターフェースは以下の通りです。

```
public interface IMultiplayerNetworkSystem
{
    event ConnectedEvent OnConnected;
    event DisconnectedEvent OnDisconnected;
    event CreatedRemotePlayerEvent OnCreatedRemotePlayer;
    event CreatedLocalPlayerEvent OnCreatedLocalPlayer;
    event DestroyedRemotePlayerEvent OnDestroyedRemotePlayer;
    bool IsConnected { get; }
    void Connect();
    void Disconnect();
    PlayerID ResolvePlayerID(object args);
    PlayerID GetPlayerID(AvatarID avatarID);
    AvatarID GetAvatarID(PlayerID playerID);
    void CreateRemotePlayerLocalPlayer(IRemotePlayer remotePlayer, object args);
    void CreatedRemotePlayer(IRemotePlayer remotePlayer, object args);
    void RemoveRemotePlayer(IRemotePlayer remotePlayer);
}
```

基本的に、ネットワークシステムを利用したい場合はインストーラで機能をインストールするだけで完了します。ConektonのデフォルトではPhotonを利用しているため、必要に応じてApp IDなどを取得してください。

Photonの設定が完了している場合は特になにもすることなく、自動的に接続を開始します。

### 主な利用ケース

IMultiplayerNetworkSystemインターフェースの主な利用ケースはイベントハンドリングでしょう。IMultiplayerNetworkSystemいにたーフェースには各タイミングごとにイベントが用意されています。

- public delegate void ConnectedEvent();
- public delegate void DisconnectedEvent();
- public delegate void CreatedRemotePlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);
- public delegate void CreatedLocalPlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);
- public delegate void DestroyedRemotePlayerEvent(IAvatar avatar, IRemotePlayer remotePlayer);


#### ConnectedEvent

サーバに接続した際のイベントです。


#### DisconnectedEvent

サーバから切断した際のイベントです。

#### CreatedRemotePlayerEvent

自分以外のプレイヤーが接続され、プレイヤーオブジェクトが生成された際のイベントです。引数にはアバター情報とリモートプレイヤー情報が渡されます。

#### CreatedLocalPlayerEvent

ローカルのリモートプレイヤーが生成されたタイミングで呼ばれるイベントです。引数に渡ってくるデータは`CreatedRemotePlayerEvent`と変わりはありませんが、ローカルの、つまり自分自身のアバターなどの情報である点が異なります。

#### DestroyedRemotePlayerEvent

リモートプレイヤーが切断し、対象プレイヤーオブジェクトが破棄された際に呼ばれるイベントです。

### アバターとリモートプレイヤー相互のIDを変換する

ネットワークシステムで利用されるリモートプレイヤーはアバターシステムと密接に関係しています。リモートプレイヤーの姿（頭や手の位置など）はアバターシステムを介して視覚化されます。

場合によっては対象アバターを操作しているリモートプレイヤーを知りたい、あるいはその逆がありえるでしょう。その場合は以下のインターフェースを利用して相互にIDを変換することができます。

```
PlayerID GetPlayerID(AvatarID avatarID);
AvatarID GetAvatarID(PlayerID playerID);
```

### Installer

`IMultiplayerNetworkSystem`の機能を利用するためには`NetworkMultiplayerInstaller`を利用ください。
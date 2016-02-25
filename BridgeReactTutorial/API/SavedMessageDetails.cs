using ProductiveRage.Immutable;

namespace BridgeReactTutorial.API
{
	public class SavedMessageDetails : IAmImmutable
	{
		public SavedMessageDetails(uint id, MessageDetails message)
		{
			this.CtorSet(_ => _.Id, id);
			this.CtorSet(_ => _.Message, message);
		}
		public uint Id { get; private set; }
		public MessageDetails Message { get; private set; }
	}
}

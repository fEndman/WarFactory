	.file	"compressed_assemblies.x86.x86.s"
	.section	.data.compressed_assemblies,"aw",@progbits
	.type	compressed_assemblies, @object
	.p2align	2
	.global	compressed_assemblies
compressed_assemblies:
	/* count */
	.long	0
	/* descriptors */
	.long	0
	.size	compressed_assemblies, 8

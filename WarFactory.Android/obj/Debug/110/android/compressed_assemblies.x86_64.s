	.file	"compressed_assemblies.x86_64.x86_64.s"
	.section	.data.compressed_assemblies,"aw",@progbits
	.type	compressed_assemblies, @object
	.p2align	3
	.global	compressed_assemblies
compressed_assemblies:
	/* count */
	.long	0
	/* descriptors */
	.zero	4
	.quad	0
	.size	compressed_assemblies, 16

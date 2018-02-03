
SOURCES=SLPrlg.cs PrlgEmbeddedPredicate.cs PrlgParser.cs PrlgEnv.cs


all: SLPrlg.exe


SLPrlg.exe: $(SOURCES) PrlgParserJay.cs
	gmcs $(SOURCES) PrlgParserJay.cs -out:SLPrlg.exe


PrlgParserJay.cs: PrlgParserJay.jay
	jay -c PrlgParserJay.jay < /usr/share/jay/skeleton.cs > PrlgParserJay.cs


install:
	cp -vf Bin/Debug/SLProlog.xap /server/http/www/top/g-pro/sfc/slrun/


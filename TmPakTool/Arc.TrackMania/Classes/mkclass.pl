use strict;

open INFILE, "TmForever objtree.txt";
my $child;
my %baseClasses = ();
while(<INFILE>)
{
	chomp;
	if(!$child && /^0000: (\w+)/)
	{
		$child = $1;
	}
	elsif($child && /^0000:     (\w+)/)
	{
		$baseClasses{$child} = $1;
		$child = undef;
	}
	else
	{
		$child = undef;
	}
}

close INFILE;

open INFILE, "engine-classes.txt";

my $engineId;
my $engineName;
while(<INFILE>)
{
	chomp;
	
	if(/^(\w\w) (\w+)/)
	{
		$engineId = $1;
		$engineName = $2;
		mkdir $engineName;
	}
	elsif(/^    (\w\w\w) (\w+)/)
	{
		open OUTFILE, ">$engineName/$2.cs";
		print OUTFILE <<"END";
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arc.TrackMania.Files.Classes.$engineName
{
    public class $2 : $baseClasses{$2}
    {
        public override uint ID
        {
            get { return 0x${engineId}${1}000; }
        }
    }
}
END
		close OUTFILE;
	}
}

close INFILE;

TYPE=TRIGGERS
triggers='CREATE DEFINER=`root`@`%` TRIGGER b_busfinger_add\nAFTER INSERT ON b_busfinger\nFOR EACH ROW\nBEGIN\n     INSERT b_busfinger_update(fingerid,opertype) VALUES(new.kid,1);\nEND' 'CREATE DEFINER=`root`@`%` TRIGGER b_busfinger_edit\nAFTER UPDATE ON b_busfinger\nFOR EACH ROW\nBEGIN\n     INSERT b_busfinger_update(fingerid,opertype) VALUES(new.kid,3);\nEND' 'CREATE DEFINER=`root`@`%` TRIGGER b_busfinger_delete\nBEFORE DELETE ON b_busfinger\nFOR EACH ROW\nBEGIN\n     INSERT b_busfinger_update(fingerid,opertype) VALUES(old.kid,2);\nEND'
sql_modes=1075838976 1075838976 1075838976
definers='root@%' 'root@%' 'root@%'
client_cs_names='utf8' 'utf8' 'utf8'
connection_cl_names='utf8_general_ci' 'utf8_general_ci' 'utf8_general_ci'
db_cl_names='utf8_general_ci' 'utf8_general_ci' 'utf8_general_ci'
